using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    #region Architecture
    public interface IArchitecture
    {
        void RegisterSystem<T>(T system) where T : ISystem;
        void RegisterModel<T>(T model) where T : IModel;
        void RegisterUtility<T>(T utility) where T : IUtility;
        T GetSystem<T>() where T : class, ISystem;
        T GetModel<T>() where T : class, IModel;
        T GetUtility<T>() where T : class, IUtility;
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
        TResult SendQury<TResult>(IQuery<TResult> query);
        void SendEvnet<T>() where T : new();
        void SendEvnet<T>(T e);
        IUnregister RegisterEvent<T>(Action<T> onEvent);
        void UnregisterEvent<T>(Action<T> onEvent);
    }

    //�q���N���X�Ɍp������邽�߂̑��݂�����A���ۃN���X�ɂȂ�
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        /// <summary>
        /// �t���C�������������t���O
        /// </summary>
        private bool mInited = false;
        //�V�X�e���������p�̃��X�g
        private List<ISystem> mSystems = new List<ISystem>();
        //Model���������X�g
        private List<IModel> mModels = new List<IModel>();
        //�������pdelegate
        public static Action<T> OnRegisterPatch = architecture => { };

        private static T mArchitecture;
        /// <summary>
        /// �C���^�t�F�[�X�`�̃C���X�^���X
        /// </summary>
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }
                return mArchitecture;
            }
        }

        /// <summary>
        /// �t���C�����k������Ȃ����m�ۊ֐�
        /// </summary>
        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                //�t���C���������֐��Ainit�͎q���N���X�Ɏ��������
                mArchitecture.Init();
                OnRegisterPatch?.Invoke(mArchitecture);
                //modle������
                foreach (var architectureModel in mArchitecture.mModels)
                {
                    architectureModel.Init();
                }
                //����������������A���g���Ȃ��������X�g�����
                mArchitecture.mModels.Clear();
                //system������
                foreach (var architectureSystem in mArchitecture.mSystems)
                {
                    architectureSystem.Init();
                }
                mArchitecture.mSystems.Clear();
                //�t���C������������
                mArchitecture.mInited = true;
            }
        }
        //��ʓI�esystem��model�̏�����������overrider�̒��ɏ����܂�
        protected abstract void Init();
        //IOC�e����̉�
        private IOCContainer mContainer = new IOCContainer();

        /// <summary>
        /// �O������IOC���l���̕��@
        /// </summary>
        /// <typeparam name="TContainer"></typeparam>
        /// <returns></returns>
        //��ʓI�ɂ͂������܂���ł����A�ꉞAPI�񋟂���
        public static TContainer Get<TContainer>() where TContainer : class
        {
            MakeSureArchitecture();
            return mArchitecture.mContainer.Get<TContainer>();
        }
        /// <summary>
        ///IOC���g�o�^
        /// </summary>
        /// <typeparam name="TContainer"></typeparam>
        /// <param name="instance"></param>
        public static void Register<TContainer>(TContainer instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register<TContainer>(instance);
        }
        /// <summary>
        /// �g����c�[���̓o�^
        /// </summary>
        /// <typeparam name="TUtility"></typeparam>
        /// <param name="utility"></param>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            mContainer.Register<TUtility>(utility);
        }
        /// <summary>
        /// �C���^�t�F�[�X�ɂ���āA�c�[���̎��̂��l��
        /// </summary>
        /// <typeparam name="TUtility"></typeparam>
        /// <returns></returns>
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return mContainer.Get<TUtility>();
        }
        //model�̓o�^
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            mContainer.Register<TModel>(model);

            if (!mInited)
            {
                mModels.Add(model);
            }
            else
            {
                model.Init();
            }

            mModels.Add(model);
        }
        /// <summary>
        /// �C���^�t�F�[�X�ɂ���āAmodel���l��
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return mContainer.Get<TModel>();
        }
        //system�o�^
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            mContainer.Register<TSystem>(system);

            if (!mInited)
            {
                mSystems.Add(system);
            }
            else
            {
                system.Init();
            }
        }
        /// <summary>
        /// �C���^�t�F�[�X�ɂ���āAsystem���l��
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        /// <returns></returns>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return mContainer.Get<TSystem>();
        }
        /// <summary>
        /// �R���g���[���[����system�܂���model�ւ̑���
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            command.SetArchitecture(this);
            command.Execute();
            command.SetArchitecture(null);
        }
        /// <summary>
        /// �R���g���[���[����system�܂���model�ւ̑���
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
            command.SetArchitecture(null);
        }

        //����system���̉�
        ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        public void SendEvnet<TEvent>() where TEvent : new()
        {
            mTypeEventSystem.Send<TEvent>();
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="e"></param>
        public void SendEvnet<TEvent>(TEvent e)
        {
            mTypeEventSystem.Send<TEvent>(e);
        }
        /// <summary>
        /// �����o�^
        /// </summary>
        /// <typeparam name="TEvent">�����̃^�C�v</typeparam>
        /// <param name="onEvent">�g���K�[���@</param>
        /// <returns></returns>
        public IUnregister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }
        /// <summary>
        /// �������O
        /// </summary>
        /// <typeparam name="TEvent">�����̃^�C�v</typeparam>
        /// <param name="onEvent">�g���K�[���@</param>
        public void UnregisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.Unregister<TEvent>(onEvent);

        }
        /// <summary>
        /// model�Ƀf�[�^�T�[�`
        /// </summary>
        /// <typeparam name="TResult">���^�[���̃f�[�^�̃^�C�v</typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public TResult SendQury<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }
    }
    #endregion

    #region Controller
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel, ICanRegisterEvent, ICanSendQuery
    {

    }
    #endregion

    #region System
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility,
        ICanSendEvent, ICanRegisterEvent,ICanGetSystem
    {
        void Init();
    }
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
        void ISystem.Init()
        {
            OnInit();
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnInit();
    }
    #endregion

    #region Model
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }
    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        void IModel.Init()
        {
            OnInit();
        }
        protected abstract void OnInit();
    }
    #endregion

    #region Utility
    public interface IUtility
    {

    }
    #endregion

    #region Command
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }
    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        void ICommand.Execute()
        {
            OnExecute();
        }
        protected abstract void OnExecute();
    }
    #endregion

    #region IQuery
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSendQuery
    {
        TResult Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }
        protected abstract T OnDo();
        private IArchitecture mArchitecture;
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion

    #region Rule
    //Architecture�ɏ�������̋L��
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchitecture();
    }
    public interface ICanSetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }
    public interface ICanGetModel : IBelongToArchitecture
    {

    }
    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }
    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
    public interface ICanGetUtility : IBelongToArchitecture
    {
    }
    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
    public interface ICanRegisterEvent : IBelongToArchitecture
    {

    }
    public static class CanRegisterEventExtension
    {
        public static IUnregister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
        public static void UnregisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnregisterEvent<T>(onEvent);
        }
    }
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
    public interface ICanSendEvent : IBelongToArchitecture
    {

    }
    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvnet<T>();
        }

        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvnet<T>(e);
        }
    }
    public interface ICanSendQuery : IBelongToArchitecture
    {

    }
    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQury(query);
        }
    }
    #endregion

    #region TypeEventSystem
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);

        IUnregister Register<T>(Action<T> onEvent);
        void Unregister<T>(Action<T> onEvent);
    }
    public interface IUnregister
    {
        void Unregister();
    }
    public struct TypeEventSystemUnregister<T> : IUnregister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;
        public void Unregister()
        {
            TypeEventSystem.Unregister<T>(OnEvent);
            TypeEventSystem = null;
            OnEvent = null;
        }
    }
    /// <summary>
    /// MonoBehaviour�̐��������ɂ���āA�����I�Ɏ�������
    /// </summary>
    public class UnregisterOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<IUnregister> mUnregistered = new HashSet<IUnregister>();

        public void AddUnregister(IUnregister unregister)
        {
            mUnregistered.Add(unregister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnregistered)
            {
                unRegister.Unregister();
            }

            mUnregistered.Clear();
        }
    }
    /// <summary>
    /// ���������ÓI�G�N�X�e���V����
    /// </summary>
    public static class UnregisterExtension
    {
        public static void UnregisterWhenGameObjectDestroyde(this IUnregister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnregisterOnDestroyTrigger>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnDestroyTrigger>();
            }
            trigger.AddUnregister(unRegister);
        }
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        public interface IRegistrations
        {

        }
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }
        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();
        public IUnregister Register<TEvent>(Action<TEvent> onEvent)
        {
            var type = typeof(TEvent);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {

            }
            else
            {
                registrations = new Registrations<TEvent>();
                mEventRegistration.Add(type, registrations);
            }
            (registrations as Registrations<TEvent>).OnEvent += onEvent;
            return new TypeEventSystemUnregister<TEvent>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }
        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }
        public void Unregister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
    #endregion

    #region IOC
    public class IOCContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }
        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.TryGetValue(key, out var reinstance))
            {
                return reinstance as T;
            }
            return null;
        }
    }
    #endregion

    #region BindableProperty
    /// <summary>
    /// �������鑮��
    /// </summary>
    /// <typeparam name="T">�����̌`</typeparam>
    public class BindableProperty<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }
        private T mValue = default(T);

        public T Value
        {
            get => mValue;
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                mOnValueChanged?.Invoke(value);
            }
        }
        public Action<T> mOnValueChanged = (v) => { };
        public IUnregister Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnregister<T>()
            {
                BindableProperty = this,
                onValueChanged = onValueChanged
            };
        }
        /// <summary>
        /// ������with�l
        /// </summary>
        /// <param name="onValueChanged">value�ύX���������̕��@</param>
        /// <returns></returns>
        public IUnregister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }
        /// <summary>
        /// ������value���B���I�Ɍ`�]��
        /// </summary>
        /// <param name="property"></param>
        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public void Unregister(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }
    }
    public class BindablePropertyUnregister<T> : IUnregister
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> onValueChanged { get; set; }
        public void Unregister()
        {
            BindableProperty.Unregister(onValueChanged);

            BindableProperty = null;
            onValueChanged = null;
        }
    }
    #endregion
}�@
