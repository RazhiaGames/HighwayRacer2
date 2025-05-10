using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Joyixir.GameManager.UI
{
	public abstract class View : MonoBehaviour
	{
		[PropertyTooltip("The thing that will animate")][SerializeField] protected Transform contentTf; 
		[GUIColor("Red")][SerializeField] protected CanvasGroup viewCanvasGroup;
		[GUIColor("Red")][SerializeField] protected CanvasGroup bgImage; 
		public UnityEvent OnViewOpened; //using for touch input right now
		public UnityEvent OnViewClosed;
		protected virtual float bgFadeEndValue => 0.8f;
		private void Update()
		{
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				OnBackBtn();
			}
		}

		protected abstract void OnBackBtn();


		public virtual void Show()
		{
			viewCanvasGroup.alpha = 1;
			bgImage.blocksRaycasts = true;
			OnViewOpened?.Invoke();
			gameObject.SetActive(true);	
		}

		public void Hide()
		{
			OnViewClosed?.Invoke();
			gameObject.SetActive(false);
		}
		public virtual void Close()
		{
			OnViewClosed?.Invoke();
			// UIManager.instance.RemoveFromWindowsInstances(this); //select chapter view has error when using, close all ui in cb
			if (gameObject != null)
			{
				Destroy(gameObject);
			}
		}


		
		// public virtual async Task AnimateUp()
		// {
		// 	contentTf.DOScale(Vector3.one, GS.INS.CBButtonsAnimateTime).From(Vector3.zero).SetEase(GS.INS.CBButtonsOnEase);
		//
		// 	await bgImage.DOFade(bgFadeEndValue, GS.INS.CBButtonsAnimateTime).From(0).SetEase(GS.INS.CBButtonsOnEase).AsyncWaitForCompletion();
		// 	AnimateUpFinished();
		// }
		
		[Button]
		public virtual async UniTask AnimateUp()
		{
			// Run both animations in parallel and wait for both to finish
			var scaleTask = contentTf.DOScale(Vector3.one, GS.INS.buttonsAnimateTime)
				.From(Vector3.zero)
				.SetEase(GS.INS.buttonsOnEase)
				.AsyncWaitForCompletion()
				.AsUniTask(); // Convert to UniTask

			var fadeTask = bgImage.DOFade(bgFadeEndValue, GS.INS.buttonsAnimateTime)
				.From(0)
				.SetEase(GS.INS.buttonsOnEase)
				.AsyncWaitForCompletion()
				.AsUniTask(); // Convert to UniTask

			// Wait for both animations to finish
			await UniTask.WhenAll(scaleTask, fadeTask);

			AnimateUpFinished();
		}



		public virtual async UniTask AnimateDown(Action onComplete = null)
		{
			// Start fade animation but don't await it immediately
			var fadeTask = bgImage.DOFade(0, GS.INS.buttonsAnimateTime)
				.SetEase(GS.INS.buttonsOffEase)
				.AsyncWaitForCompletion()
				.AsUniTask(); // Convert to UniTask

			// Await scaling animation (ensures it finishes before proceeding)
			await contentTf.DOScale(Vector3.zero, GS.INS.buttonsAnimateTime)
				.From(Vector3.one)
				.SetEase(GS.INS.buttonsOffEase)
				.AsyncWaitForCompletion()
				.AsUniTask();

			// Ensure fade animation is completed before moving forward
			await fadeTask;

			AnimateDownFinished();
			onComplete?.Invoke();
		}


		
		public virtual async UniTask FadeIn()
		{
			await viewCanvasGroup.DOFade(1, GS.INS.buttonsAnimateTime)
				.From(0)
				.SetEase(GS.INS.fadeEase)
				.AsyncWaitForCompletion()
				.AsUniTask(); // Convert to UniTask

			AnimateUpFinished();
		}





		public virtual async void FadeOut()
		{
			await viewCanvasGroup.DOFade(0, GS.INS.buttonsAnimateTime).From(1).SetEase(GS.INS.fadeEase).AsyncWaitForCompletion();
			AnimateDownFinished();
		}


		protected virtual void AnimateUpFinished()
		{
			bgImage.blocksRaycasts = true;
		}
			
		protected virtual void AnimateDownFinished()
		{
			bgImage.blocksRaycasts = false;
			Hide();
		}

	}
}
